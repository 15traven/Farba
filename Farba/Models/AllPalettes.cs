using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace Farba.Models
{
    internal class AllPalettes
    {
        public ObservableCollection<Palette> Palettes { get; set; } = [];

        public AllPalettes()
        {
            LoadPalettes();
        }

        public async void LoadPalettes()
        {
            Palettes.Clear();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            await GetFilesInFolderAsync(storageFolder);
        }

        private static async Task GetFilesInFolderAsync(StorageFolder storageFolder)
        {
            IReadOnlyList<IStorageItem> storageItems = await storageFolder.GetFilesAsync();

            foreach (IStorageItem storageItem in storageItems)
            {
                if (storageItem.IsOfType(StorageItemTypes.File))
                {
                    StorageFile file = (StorageFile)storageItem;
                    Palette palette = new()
                    {
                        PaletteName = Path.GetFileNameWithoutExtension(file.Name),
                        ColorsHex = JsonSerializer.Deserialize<List<string>>(await FileIO.ReadTextAsync(file))
                    };
                }
            }
        }
    }
}
