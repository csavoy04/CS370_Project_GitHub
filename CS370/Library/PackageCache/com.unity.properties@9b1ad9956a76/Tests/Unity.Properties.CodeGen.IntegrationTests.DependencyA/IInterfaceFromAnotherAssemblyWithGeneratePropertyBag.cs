using Unity.Properties;

[assembly: GeneratePropertyBagsForTypesQualifiedWith(typeof(IInterfaceFromAnotherAssemblyWithGeneratePropertyBag), TypeGenerationOptions.ReferenceType)]

public interface IInterfaceFromAnotherAssemblyWithGeneratePropertyBag
{
    
}