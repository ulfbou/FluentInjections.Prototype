// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


namespace FluentInjections.Exceptions
{
    [Serializable]
    internal class ComponentContractMismatchException : Exception
    {
        public Type ContractType { get; }
        public Type ActualType { get; }

        public ComponentContractMismatchException(Type contractType, Type type) : base($"The contract type {contractType} does not match the component type {type}.")
        {
            ContractType = contractType;
            ActualType = type;
        }
    }
}
