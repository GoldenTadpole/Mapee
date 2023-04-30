using AssetSystem;
using AssetSystem.Block;
using Mapper.Gui.Model;
using MapScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for BlockFilter.xaml
    /// </summary>
    public partial class BlockFilterControl : Window
    {
        public IScanProfile ScanProfile { get; private set; }

        public event EventHandler? BlockFilterUpdated;
        public bool DialogClosed { get; private set; } = true;

        private readonly IDictionary<string, BlockEntry<BlockGrouping>> _solids;
        private readonly IDictionary<string, BlockEntry<BlockGrouping>> _transparent;
        private readonly SearchableBlocks _solidSearchableBlocks, _transparentSearchableBlocks;

        public BlockFilterControl(IScanProfile scanProfile)
        {
            InitializeComponent();

            ScanProfile = scanProfile;

            _solids = Seperate(ScanProfile.Asset, BlockType.StopAtEncounter);
            _transparent = Seperate(ScanProfile.Asset, BlockType.SemiTransparent);

            _solidSearchableBlocks = new SearchableBlocks(_solids.Values.ToList());
            SolidBlockView.ItemsSource = _solidSearchableBlocks;

            _transparentSearchableBlocks = new SearchableBlocks(_transparent.Values.ToList());
            TransparentBlockView.ItemsSource = _transparentSearchableBlocks;

            List<string> heightmaps = new List<string>(ScanProfile.AllowedNbtHeightmaps ?? Enumerable.Empty<string>());
            if (!heightmaps.Contains(ScanProfile.HeightmapProfile.NbtHeightmap)) 
            {
                heightmaps.Add(ScanProfile.HeightmapProfile.NbtHeightmap);
            }

            HeightmapComboBox.ItemsSource = heightmaps;
            HeightmapComboBox.SelectedItem = ScanProfile.HeightmapProfile.NbtHeightmap;

            HeightmapRadioButton.IsChecked = ScanProfile.HeightmapProfile.HeightmapType == HeightmapType.NbtHeightmap;
            SetYRadioButton.IsChecked = ScanProfile.HeightmapProfile.HeightmapType == HeightmapType.SetY;
            SetYTextBox.Text = ScanProfile.HeightmapProfile.SetY.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = new CustomFrameWindowInitializer(this, Template);

            SetHeightmapAvailability();
        }

        private static IDictionary<string, BlockEntry<BlockGrouping>> Seperate(ITokenizedBlockAsset<BlockGrouping> asset, BlockType type)
        {
            Dictionary<string, BlockEntry<BlockGrouping>> output = new();

            foreach (KeyValuePair<string, BlockEntry<BlockGrouping>> pair in asset.Blocks)
            {
                if (!ContainsSpecificType(pair.Value, type)) continue;

                BlockEntry<BlockGrouping> entry = (BlockEntry<BlockGrouping>) pair.Value.Clone();
                KeepOnlySpecificType(entry, type);

                output.Add(pair.Key, entry);
            }

            return output;
        }
        private static bool ContainsSpecificType(BlockEntry<BlockGrouping> entry, BlockType type)
        {
            if (entry.DefaultValue is not null && entry.DefaultValue.Value.Type == type) return true;

            foreach (PropertyMatcher<BlockGrouping> propertyMatcher in entry.Evaluators)
            {
                if (propertyMatcher.Payload.Type == type) return true;
            }

            return false;
        }
        private static void KeepOnlySpecificType(BlockEntry<BlockGrouping> entry, BlockType type) 
        {
            if (entry.DefaultValue is not null && entry.DefaultValue.Value.Type != type) 
            {
                entry.DefaultValue = null;
            }

            for (int i = 0; i < entry.Evaluators.Count; i++) 
            {
                if (entry.Evaluators[i].Payload.Type != type) 
                {
                    entry.Evaluators.RemoveAt(i--);
                }
            }
        }

        private void SolidBlockSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = SolidBlockSearch.Text.ToLower();

            _solidSearchableBlocks.Search(text);
            SolidBlockView.Items.Refresh();
        }
        private void TransparentBlockSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = TransparentBlockSearch.Text.ToLower();

            _transparentSearchableBlocks.Search(text);
            TransparentBlockView.Items.Refresh();
        }

        private void SolidBlockRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _solidSearchableBlocks.Remove(SolidBlockView.SelectedItems.Cast<BlockEntry<BlockGrouping>>(), BlockType.StopAtEncounter);
            SolidBlockView.Items.Refresh();
        }
        private void TransparentBlockRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _transparentSearchableBlocks.Remove(TransparentBlockView.SelectedItems.Cast<BlockEntry<BlockGrouping>>(), BlockType.SemiTransparent);
            TransparentBlockView.Items.Refresh();
        }

        private void ClearSolidBlocksButton_Click(object sender, RoutedEventArgs e)
        {
            _solidSearchableBlocks.ClearEverything(BlockType.StopAtEncounter);
            SolidBlockView.Items.Refresh();
        }
        private void ClearTransparntBlocksButton_Click(object sender, RoutedEventArgs e)
        {
            _transparentSearchableBlocks.ClearEverything(BlockType.SemiTransparent);
            TransparentBlockView.Items.Refresh();
        }

        private void SolidBlocksResetToDefault_Click(object sender, RoutedEventArgs e)
        {
            _solidSearchableBlocks.ResetToDefault(Seperate(ScanProfile.DefaultAsset ?? TokenizedBlockAsset<BlockGrouping>.Empty, BlockType.StopAtEncounter).Values.ToList());
            SolidBlockView.Items.Refresh();
        }
        private void TransparentBlocksResetToDefault_Click(object sender, RoutedEventArgs e)
        {
            _transparentSearchableBlocks.ResetToDefault(Seperate(ScanProfile.DefaultAsset ?? TokenizedBlockAsset<BlockGrouping>.Empty, BlockType.SemiTransparent).Values.ToList());
            TransparentBlockView.Items.Refresh();
        }

        private void SolidBlockAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddBlock(SolidBlockAddButton, SolidBlockView, _solidSearchableBlocks, BlockType.StopAtEncounter);
        }
        private void TransparentBlockAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddBlock(TransparentBlockAddButton, TransparentBlockView, _transparentSearchableBlocks, BlockType.SemiTransparent);
        }
        private static void AddBlock(Control button, DataGrid dataGrid, SearchableBlocks container, BlockType type)
        {
            Point startupLocation = button.PointToScreen(new(0, 0)).CalibrateToDpiScale();

            AddBlock goToWindow = new("minecraft:");

            startupLocation.Y += button.ActualHeight;
            startupLocation.X += button.ActualWidth / 2 - goToWindow.Width / 2;

            goToWindow.Top = startupLocation.Y;
            goToWindow.Left = startupLocation.X;

            goToWindow.Show();
            goToWindow.Closing += (s, e) =>
            {
                if (goToWindow.DialogClosed) return;

                dataGrid.Dispatcher.Invoke(() =>
                {
                    BlockEntry<BlockGrouping> entry = new(goToWindow.NameResult) 
                    {
                        DefaultValue = new BlockGrouping(type)
                    };
                    container.Add(entry);

                    dataGrid.Items.Refresh();
                    dataGrid.SelectedItem = entry;
                    dataGrid.UpdateLayout();
                    dataGrid.ScrollIntoView(entry);
                });
            };
        }

        private void HeightmapRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SetHeightmapAvailability();
        }
        private void SetYRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SetHeightmapAvailability();
        }
        private void SetHeightmapAvailability()
        {
            if (HeightmapComboBox is null || SetYRadioButton is null || SetYTextBox is null) return;

            HeightmapComboBox.IsEnabled = HeightmapRadioButton.IsChecked ?? false;
            SetYTextBox.IsEnabled = SetYRadioButton.IsChecked ?? false;
        }

        private void RenderButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            ScanProfile.HeightmapProfile = CreateHeightmapProfile();
            ScanProfile.Asset = CreateAsset();

            DialogClosed = false;
            BlockFilterUpdated?.Invoke(this, EventArgs.Empty);
            if(CloseWindowCheckBox.IsChecked ?? true) Close();
        }

        private HeightmapSettings CreateHeightmapProfile()
        {
            return new HeightmapSettings()
            {
                HeightmapType = (HeightmapRadioButton.IsChecked ?? false) ? HeightmapType.NbtHeightmap : HeightmapType.SetY,
                SetY = short.Parse(SetYTextBox.Text),
                NbtHeightmap = HeightmapComboBox.SelectedItem as string ?? string.Empty
            };
        }
        private ITokenizedBlockAsset<BlockGrouping> CreateAsset()
        {
            ITokenizedBlockAsset<BlockGrouping> output = ScanProfile.Asset ?? TokenizedBlockAsset<BlockGrouping>.Empty;
            output.Blocks.Clear();

            foreach (BlockEntry<BlockGrouping> entry in _solidSearchableBlocks.OriginalBlockList)
            {
                if (output.Blocks.ContainsKey(entry.BlockName)) continue;
                output.Blocks.Add(entry.BlockName, entry);
            }

            foreach (BlockEntry<BlockGrouping> entry in _transparentSearchableBlocks.OriginalBlockList)
            {
                if (output.Blocks.ContainsKey(entry.BlockName)) continue;
                output.Blocks.Add(entry.BlockName, entry);
            }

            return output;
        }

        private bool ValidateInput() 
        {
            if (!InputValidator.ValidateInt16(SetYTextBox.Text)) 
            {
                return InvalidInput.ShowMessage("Set Y must be a number.");
            }

            return true;
        }
    }
}
