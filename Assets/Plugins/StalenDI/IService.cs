using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stalen.DI
{
    public interface IService : IDependent
    {
        string Name { get; }
    }
}