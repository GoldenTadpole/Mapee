using WorldEditor;

namespace Mapper
{
    public class WorldMapper
    {
        public MapperPack MapperPack { get; }

        public IChunkEnumerator ChunkEnumerator { get; }
        protected WorldMapperEnumerationBody EnumerationBody { get; }

        public Action<Coords, ICanvas>? RegionRendered
        {
            get => EnumerationBody.RegionRendered;
            set => EnumerationBody.RegionRendered = value;
        }
        public SceneInfo CurrentScene { get; protected set; }
        public IQueue<string> Queue { get; }

        private Action? _invoke;
        private bool _newInvoke = false;
        private object _invokeLock = new();

        private static readonly int REGIONS_IN_PARALLEL = 4;
        private static readonly int CHUNKS_IN_PARALLEL = 8;

        public WorldMapper(MapperPack mapperPack)
        {
            MapperPack = mapperPack;

            ChunkEnumeratorFromRegionFactory factory = new(index => MapperPack.ChunkReader);
            ChunkEnumerator = new ChunkEnumerator(REGIONS_IN_PARALLEL, CHUNKS_IN_PARALLEL, factory);

            EnumerationBody = new WorldMapperEnumerationBody(MapperPack, REGIONS_IN_PARALLEL, CHUNKS_IN_PARALLEL);
            Queue = new SynchronizedQueue();

            InitializeThread();
        }

        public virtual void SetScene(SceneInfo scene)
        {
            CurrentScene = scene;

            Stop();
            EnumerationBody.ResetScene();
        }
        public void Stop()
        {
            Queue.ReplaceWith(ReadOnlyMemory<string>.Empty);
        }

        private void InitializeThread()
        {
            new Thread(() =>
            {
                while (true)
                {
                    EnumerateThroughCache();
                    Thread.Sleep(5);
                }
            })
            {
                IsBackground = true
            }.Start();
        }
        protected virtual void EnumerateThroughCache()
        {
            while (Queue.Count > 0)
            {
                ChunkEnumerator.Enumerate(Queue.TakeFirst(REGIONS_IN_PARALLEL), EnumerationBody);
                TriggerInvoke();
            }

            TriggerInvoke();
        }

        private void TriggerInvoke()
        {
            Action? invoke = _invoke;
            if (invoke is null) return;

            invoke();
            lock (_invokeLock)
            {
                if (_newInvoke) _newInvoke = false;
                else _invoke = null;
            }
        }

        public void Invoke(Action action)
        {
            lock (_invokeLock) 
            {
                _invoke = action;
                _newInvoke = true;
            }
        }
    }
}
