using System.Diagnostics;
using System.Reflection;

using Dumpify;

using Net.Belt.Tests.Support;

using NUnit.Framework.Internal;

namespace Net.Belt.Tests;

[TestFixture, Category("showcase")]
public class EnumerationShowcase
{
	[Test]
	public void Definition()
	{
		PlatformID platform = PlatformID.MacOSX;
		platform.Dump("Defined");
	
		platform = (PlatformID)256;
		platform.Dump("Undefined, doh!");
	
		Enum.IsDefined(typeof(PlatformID), platform).Dump("Oh, OK, it is undefined. And boxed too");
		Enumeration.IsDefined(platform).Dump("shorter");
	
		Enumeration.IsDefined<PlatformID>("MacOSX").Dump("value defined");
		Enumeration.IsDefined<PlatformID>("Macosx", true).Dump("value defined");
		Enumeration.IsDefined<PlatformID>(256).Dump("value undefined");
		Enumeration.IsDefined<PlatformID>(5).Dump("numeric value defined");
	}

	[Test]
	public void Values()
	{
		Enumeration.GetUnderlyingType<PlatformID>().Dump("That's my type");
	
		Enumeration.GetValues<PlatformID>().Dump("All values");
	
		Enumeration.Omit(PlatformID.Xbox).Dump("Consoles are NOT computers!");
	
		byte small = Enumeration.GetValue<PlatformID, byte>(PlatformID.MacOSX).Dump("Numeric type conversions apply");
		//Enumeration.GetValue<PlatformID, byte>((PlatformID)256).Dump("Exception for undefined values");
	
		Enumeration.TryGetValue((PlatformID)256, out byte? maybe).Dump("Nope");
		maybe.Dump("No value for undefined");
	}

	[Test]
	public void ToEnum()
	{
		Enumeration.Cast<PlatformID>(6).Dump();
	
		PlatformID? casted;
		Enumeration.TryCast(256, out casted).Dump("Cannot cast");
		casted.Dump("I told you I could not cast");
	
		//Enumeration.Cast<PlatformID>(256);
	
		Enumeration.Parse<PlatformID>("MacOSX").Dump("Case-sensitive");
	
		Enumeration.TryParse("6", out PlatformID? parsed).Dump("Parse values");
		parsed.Dump("Works with values too");
	}

	[Test]
	public void Reflection()
	{
		Enumeration.GetField(MyEnum.One).Dump("A field");
	
		Enumeration.HasAttribute<MyEnum, MonitoringDescriptionAttribute>(MyEnum.Three).Dump("Check attributes");
	
		Enumeration.GetAttribute<MyEnum, ObfuscationAttribute>(MyEnum.Three).Dump("Get attribute");
	}

	[Test]
	public void Flags()
	{
		Enumeration.IsFlags<FlagEnum>().Dump("Check");
		// True

		FlagEnum.One
			.SetFlag(FlagEnum.Four).Dump("Set")
			.UnsetFlag(FlagEnum.One).Dump("Unset")
			.ToggleFlag(FlagEnum.One).Dump("Toggle on")
			.ToggleFlag(FlagEnum.One).Dump("Toggle off")
			.GetFlags().Dump("Include Zero");

		(FlagEnum.One | FlagEnum.Four).GetFlags(ignoreZeroValue: true).Dump("Do not include zero");
	}
}