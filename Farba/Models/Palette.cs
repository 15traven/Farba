using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.Storage;

namespace Farba.Models
{
    internal class Palette
    {
        private StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        public string PaletteName { get; set; } = string.Empty;
        public List<string> ColorsHex { get; set; } = [];

        public Palette(string Name, List<string> colors)
        {
            PaletteName = Name;
            ColorsHex = colors;
        }

        public Palette()
        {
            PaletteName = "New palette";
            ColorsHex = [];
        }

        public async Task SaveAsync()
        {
            StorageFile paletteFile = (StorageFile)await storageFolder.TryGetItemAsync(PaletteName + ".json");
            paletteFile ??= await storageFolder.CreateFileAsync(PaletteName + ".json", CreationCollisionOption.ReplaceExisting);

            string jsonData = JsonSerializer.Serialize(ColorsHex);
            await FileIO.WriteTextAsync(paletteFile, jsonData);
        }

        public async Task DeleteAsync()
        {
            StorageFile paletteFile = (StorageFile)await storageFolder.TryGetItemAsync(PaletteName + ".json");
            if (paletteFile is not null)
                await paletteFile.DeleteAsync();
        }
    }
}
