using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cathei.BakingSheet;

namespace Core.Data
{
    public class GameModeSheet : Sheet<GameModeSheet.Row>
    {
        public class Row : SheetRow
        {
            public VerticalList<float> Experience { get; private set; }
            //public VerticalList<float> SpawnRate { get; private set; }
            //public VerticalList<string> AddToSpawn { get; private set; }
            public VerticalList<string> SingleSpawn { get; private set; }
        }
    }
}