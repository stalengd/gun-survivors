using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cathei.BakingSheet;

namespace Core.Data
{
    public class UpgradesSheet : Sheet<UpgradesSheet.Row>
    {
        public class Row : SheetRow
        {
            public List<float> Values { get; private set; }
        }
    }
}