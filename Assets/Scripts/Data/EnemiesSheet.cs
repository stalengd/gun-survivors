using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cathei.BakingSheet;

namespace Core.Data
{
    public class EnemiesSheet : Sheet<EnemiesSheet.Row>
    {
        public class Row : SheetRow
        {
            public VerticalList<float> SpawnRate { get; private set; }
            public VerticalList<float> Health { get; private set; }
            public VerticalList<float> Speed { get; private set; }
        }
    }
}