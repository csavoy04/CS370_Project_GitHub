using UnityEngine.Scripting;

namespace Unity.Properties.Tests
{
    public interface IConstructInterface
    {

    }

    public abstract class AbstractConstructibleBaseType : IConstructInterface
    {

    }

    public class ConstructibleBaseType : AbstractConstructibleBaseType
    {
        public float Value;

        [Preserve]
        public ConstructibleBaseType()
        {
            Value = 25.0f;
        }
    }

    public class ConstructibleDerivedType : ConstructibleBaseType
    {
        public float SubValue;

        [Preserve]
        public ConstructibleDerivedType()
            :base()
        {
            SubValue = 50.0f;
        }
    }

    public class NonConstructibleDerivedType : ConstructibleBaseType
    {
        public NonConstructibleDerivedType(float a)
        {
        }
    }

    [Preserve]
    public class NoConstructorType
    {

    }

    public class ParameterLessConstructorType
    {
        public float Value;

        [Preserve]
        public ParameterLessConstructorType()
        {
            Value = 25.0f;
        }
    }

    public class ParameterConstructorType
    {
        public float Value;

        [Preserve]
        public ParameterConstructorType(float a)
        {
            Value = a;
        }
    }

    [Preserve]
    public class ScriptableObjectType : UnityEngine.ScriptableObject
    {
    }
}
