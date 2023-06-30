using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace Core.Data
{
    public class SheetContainer : SheetContainerBase
    {
        public SheetContainer(Microsoft.Extensions.Logging.ILogger logger) : base(logger) { }

        public UpgradesSheet Upgrades { get; private set; }
        public EnemiesSheet Enemies { get; private set; }
        public GameModeSheet GameMode { get; private set; }

#if UNITY_EDITOR
        [MenuItem("Tools/Load Spreadsheets")]
        public async static void CreateScriptableObject()
        {
            var logger = new UnityLogger();
            var sheetContainer = new SheetContainer(logger);

            string googleSheetId = "10M-O63Sj7Vbn7o4kftogid-CkbclbEZLeQdz0kgLHAM";

            string googleCredential = File.ReadAllText("unity-spreadsheets-386114-266beb0ee647.json");

            var googleConverter = new GoogleSheetConverter(googleSheetId, googleCredential);

            await sheetContainer.Bake(googleConverter);

            var exporter = new ScriptableObjectSheetExporter("Assets/Resources/Data");

            await sheetContainer.Store(exporter);
        }
#endif
    }
}