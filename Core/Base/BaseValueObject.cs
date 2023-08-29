using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base;

public abstract class BaseValueObject<TValueObject> : IEquatable<TValueObject> where TValueObject : BaseValueObject<TValueObject>
{
    public override bool Equals(object obj)
    {
        TValueObject val = obj as TValueObject;
        if (val == null)
        {
            return false;
        }

        return ObjectIsEqual(val);
    }

    public override int GetHashCode()
    {
        return (from x in GetEqualityComponents()
                select x?.GetHashCode() ?? 0).Aggregate((x, y) => x ^ y);
    }

    public abstract bool ObjectIsEqual(TValueObject otherObject);

    protected abstract IEnumerable<object?> GetEqualityComponents();

    public abstract int ObjectGetHashCode();

    public bool Equals(TValueObject obj)
    {
        if (obj.GetType() != GetType())
        {
            return false;
        }

        return GetEqualityComponents().SequenceEqual(obj.GetEqualityComponents());
    }

    public static bool operator ==(BaseValueObject<TValueObject>? right, BaseValueObject<TValueObject>? left)
    {
        if ((object)right == null && (object)left == null)
        {
            return true;
        }

        if ((object)right == null || (object)left == null)
        {
            return false;
        }

        return right!.Equals(left);
    }

    public static bool operator !=(BaseValueObject<TValueObject> right, BaseValueObject<TValueObject> left)
    {
        return !(right == left);
    }
}
