using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public sealed class CompositeFloat
    {
        public float BaseValue => baseValue;
        public float Value => computedValue;

        private float baseValue;
        private float computedValue;
        private int nextId = 1;
        private List<Modification> additions;
        private List<Modification> multiplications;


        public CompositeFloat(float baseValue = 0f)
        {
            this.baseValue = baseValue;
            computedValue = baseValue;
            nextId = 0;
            additions = null;
            multiplications = null;
        }


        public void SetBaseValue(float value)
        {
            baseValue = value;
            CalculateValue();
        }

        public CompositeToken Add(float value)
        {
            return Modify(ref additions, value);
        }

        public CompositeToken Add(ref CompositeToken token, float value)
        {
            return Modify(ref token, ref additions, value);
        }

        public CompositeToken Multiply(float value)
        {
            return Modify(ref multiplications, value);
        }

        public CompositeToken Multiply(ref CompositeToken token, float value)
        {
            return Modify(ref token, ref multiplications, value);
        }

        public bool RemoveModification(CompositeToken token)
        {
            var id = token.Id;

            if (additions != null)
            {
                for (int i = 0; i < additions.Count; i++)
                {
                    if (additions[i].id == id)
                    {
                        additions.RemoveAt(i);
                        CalculateValue();
                        return true;
                    }
                }
            }
            if (multiplications != null)
            {
                for (int i = 0; i < multiplications.Count; i++)
                {
                    if (multiplications[i].id == id)
                    {
                        multiplications.RemoveAt(i);
                        CalculateValue();
                        return true;
                    }
                }
            }
            return false;
        }

        public void ClearModifications()
        {
            additions?.Clear();
            multiplications?.Clear();
        }


        private CompositeToken Modify(ref List<Modification> modifications, float value)
        {
            var token = IssueToken();

            modifications ??= new List<Modification>();
            modifications.Add(new(token.Id, value));

            CalculateValue();

            return token;
        }

        private CompositeToken Modify(ref CompositeToken token, ref List<Modification> modifications, float value)
        {
            modifications ??= new List<Modification>();
            var id = token.Id;

            if (id == 0)
            {
                token = IssueToken();
                modifications.Add(new(token.Id, value));
            }
            else
            {
                for (int i = 0; i < modifications.Count; i++)
                {
                    var mod = modifications[i];
                    if (mod.id == id)
                    {
                        mod.value = value;
                        modifications[i] = mod;
                        break;
                    }
                }
            }

            CalculateValue();

            return token;
        }

        private CompositeToken IssueToken()
        {
            return new CompositeToken(nextId++);
        }

        private void CalculateValue()
        {
            var value = baseValue;
            if (additions != null)
            {
                for (int i = 0; i < additions.Count; i++)
                {
                    value += additions[i].value;
                }
            }
            if (multiplications != null)
            {
                for (int i = 0; i < multiplications.Count; i++)
                {
                    value *= multiplications[i].value;
                }
            }
            computedValue = value;
        }

        private struct Modification
        {
            public int id;
            public float value;

            public Modification(int id, float value)
            {
                this.id = id;
                this.value = value;
            }
        }
    }

    public readonly struct CompositeToken
    {
        public int Id { get; }

        public CompositeToken(int id)
        {
            Id = id;
        }
    }
}