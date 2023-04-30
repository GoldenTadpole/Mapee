using AssetSystem;
using AssetSystem.Block;
using MapScanner;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mapper.Gui
{
    public class SearchableBlocks : IEnumerable
    {
        public ObservableCollection<BlockEntry<BlockGrouping>> DisplayedBlockList { get; set; }
        public IList<BlockEntry<BlockGrouping>> OriginalBlockList { get; set; }
        public string? SearchName { get; private set; }

        public SearchableBlocks(IList<BlockEntry<BlockGrouping>> list)
        {
            DisplayedBlockList = new ObservableCollection<BlockEntry<BlockGrouping>>(new List<BlockEntry<BlockGrouping>>(list));
            OriginalBlockList = list;
        }

        public void Search(string searchName)
        {
            SearchName = searchName;
            Display();
        }
        public void Remove(IEnumerable<BlockEntry<BlockGrouping>> blocks, BlockType type)
        {
            foreach (BlockEntry<BlockGrouping> entry in blocks)
            {
                RemoveBlockType(entry, type);
                OriginalBlockList.Remove(entry);
            }

            Display();
        }
        public void ClearEverything(BlockType type)
        {
            Remove(new List<BlockEntry<BlockGrouping>>(OriginalBlockList), type);
        }
        public void ResetToDefault(IList<BlockEntry<BlockGrouping>> list)
        {
            OriginalBlockList = list;
            Display();
        }

        public void Add(BlockEntry<BlockGrouping> block)
        {
            OriginalBlockList.Add(block);
            DisplayedBlockList.Add(block);
        }

        private void Display()
        {
            DisplayedBlockList.Clear();
            foreach (BlockEntry<BlockGrouping> entry in OriginalBlockList)
            {
                if (MatchesSearch(entry, SearchName))
                {
                    DisplayedBlockList.Add(entry);
                }
            }
        }
        private static bool MatchesSearch(BlockEntry<BlockGrouping> entry, string? searchName)
        {
            if (string.IsNullOrEmpty(searchName)) return true;
            return entry.BlockName.Contains(searchName) || searchName.Contains(entry.BlockName);
        }

        private static void RemoveBlockType(BlockEntry<BlockGrouping> entry, BlockType type)
        {
            for (int i = 0; i < entry.Evaluators.Count; i++)
            {
                PropertyMatcher<BlockGrouping> matcher = entry.Evaluators[i];
                if (matcher.Payload.Type != type) continue;

                entry.Evaluators.RemoveAt(i--);
            }

            if (entry.DefaultValue is null || entry.DefaultValue.Value.Type != type) return;
            entry.DefaultValue = new BlockGrouping(BlockType.Disabled);
        }

        public IEnumerator GetEnumerator()
        {
            return DisplayedBlockList.GetEnumerator();
        }
    }
}
