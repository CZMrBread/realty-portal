using Shared.SRealtyRealty.ValueObjects;
using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.Interfaces;

public interface ISRealtyProperty : IPropertyCore, IPropertyFeatures, IPropertyConstruction, IPropertyUtilities,
    IPropertyLocation, IPropertyEnergyPerformance
{
}