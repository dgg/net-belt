using Net.Belt.ValueObjects.Extensions;

namespace Net.Belt.Tests.ValueObjects;

using System.Globalization;

using Net.Belt.ValueObjects;

using NUnit.Framework;

[TestFixture]
public class PercentageTester
{
	[Test]
	public void Ctor_SetsValue()
	{
		var singlePercentage = new Percentage<float>(50f);
		Assert.That(singlePercentage.Value, Is.EqualTo(50F).Within(0.0001f));

		var doublePercentage = new Percentage<double>(50.0d);
		Assert.That(doublePercentage.Value, Is.EqualTo(50D).Within(0.0001d));

		var decimalPercentage = new Percentage<decimal>(50.5m);
		Assert.That(decimalPercentage.Value, Is.EqualTo(50.5M).Within(0.0001M));
	}

	[Test]
	public void Ctor_SetsFraction()
	{
		var singlePercentage = new Percentage<float>(50f);
		Assert.That(singlePercentage.Fraction, Is.EqualTo(.5).Within(0.0001f));

		var doublePercentage = new Percentage<double>(50.0d);
		Assert.That(doublePercentage.Fraction, Is.EqualTo(.5).Within(0.0001d));

		var decimalPercentage = new Percentage<decimal>(50.5m);
		Assert.That(decimalPercentage.Fraction, Is.EqualTo(.505M).Within(0.0001M));
	}

	[Test]
	public void FromFraction_SetsPropsCorrectly()
	{
		Percentage<double> doublePercentage = Percentage<double>.FromFraction(0.25d);
		Assert.That(doublePercentage.Fraction, Is.EqualTo(0.25d).Within(0.0001d));
		Assert.That(doublePercentage.Value, Is.EqualTo(25d).Within(0.0001d));

		Percentage<decimal> decimalPercentage = Percentage<decimal>.FromFraction(0.75m);
		Assert.That(decimalPercentage.Fraction, Is.EqualTo(0.75m).Within(0.0001m));
		Assert.That(decimalPercentage.Value, Is.EqualTo(75m).Within(0.0001m));
	}

	[Test, SetCulture("en-US")]
	public void ToString_NoArgs_enUSDependentPercentFormat()
	{
		var percentage = new Percentage<double>(12.34d);
		string result = percentage.ToString();
		Assert.That(result, Is.EqualTo("12.340%"));
	}

	[Test, SetCulture("es-ES")]
	public void ToString_NoArgs_esESDependentPercentFormat()
	{
		var percentage = new Percentage<double>(12.34d);
		string result = percentage.ToString();
		Assert.That(result, Is.EqualTo("12,340 %"));
	}

	[Test, SetCulture("en-US")]
	public void ToString_CustomPercentFormat_enUSDependentCustomFormat()
	{
		string customFormat = "P2";
		string actual = new Percentage<double>(12.34d)
			.ToString(customFormat);
		Assert.That(actual, Is.EqualTo("12.34%"));
	}

	[Test, SetCulture("es-ES")]
	public void ToString_CustomPercentFormat_esESDependentCustomFormat()
	{
		string customFormat = "P1";
		string actual = new Percentage<double>(12.39d)
			.ToString(customFormat);
		Assert.That(actual, Is.EqualTo("12,4 %"));
	}

	[Test, SetCulture("es-ES")]
	public void ToString_CustomPercentFormatter_LocalizationAgnosticCustomFormat()
	{
		var formatter = new NumberFormatInfo
		{
			PercentPositivePattern = 3, // "% n"
			PercentDecimalSeparator = "|", // not ',' as in es-ES
			PercentDecimalDigits = 4,
			PercentSymbol = "pct" // '%' => 'pct'
		};
		string customPercentFormat = "P1";

		string actual = new Percentage<double>(12.39d)
			.ToString(customPercentFormat, formatter);

		Assert.That(actual, Is.EqualTo("pct 12|4"), "format (1) override formatter's PercentDecimalDigits (2)");
	}

	[Test]
	public void ToString_NumericFormatter_NoPercentSymbol()
	{
		var formatter = new NumberFormatInfo
		{
			NumberDecimalSeparator = "|",
			NumberDecimalDigits = 4
		};
		string customNumericFormat = "F2";
		string actual = new Percentage<double>(12.34d)
			.ToString(customNumericFormat, formatter);

		Assert.That(actual, Is.EqualTo("12|34"), "format (2) override formatter's NumberDecimalDigits (4)");
	}

	[Test]
	public void ToString_CustomFormat_AppliesFormatToValue()
	{
		var formatter = new NumberFormatInfo
		{
			NumberDecimalSeparator = "|"
		};
		string customNumericFormat = " 000.###";
		string actual = new Percentage<double>(12.34d)
			.ToString(customNumericFormat, formatter);

		Assert.That(actual, Is.EqualTo(" 012|34"), "format (2) override formatter's NumberDecimalDigits (4)");
	}

	[Test]
	public void CompareTo_WorksAsExpected()
	{
		var p1 = new Percentage<double>(25.0d);
		var p2 = new Percentage<double>(50.0d);
		var p3 = new Percentage<double>(25.0d);

		Assert.That(p1.CompareTo(p2), Is.LessThan(0), "p1 < p2");
		Assert.That(p2.CompareTo(p1), Is.GreaterThan(0), "p2 > p1");
		Assert.That(p1.CompareTo(p3), Is.EqualTo(0), "p1 == p3");
	}

	[Test]
	public void ComparisonOperators_WorkAsExpected()
	{
		var p1 = new Percentage<double>(30.0d);
		var p2 = new Percentage<double>(60.0d);
		var p3 = new Percentage<double>(30.0d);

		Assert.That(p1 < p2, Is.True, "p1 < p2");
		Assert.That(p2 > p1, Is.True, "p2 > p1");
		Assert.That(p1 <= p3, Is.True, "p1 <= p3");
		Assert.That(p1 >= p3, Is.True, "p1 >= p3");
	}

	[Test]
	public void Apply_Integral_PercentageApplied()
	{
		var percentage = new Percentage<decimal>(20);
		decimal actual = percentage.Apply(200L);

		Assert.That(actual, Is.EqualTo(40m));
	}

	[Test]
	public void DeductFrom_CalculatesAnAmountWithoutThePercentage()
	{
		var actual = new Percentage<double>(25d).DeductFrom(100d);
		Assert.That(actual, Is.EqualTo(80d));
	}

	[Test]
	public void FromAmounts_CalculatesPercentage()
	{
		Percentage<double> eightyPercent = Percentage<double>.FromAmounts(60L, 75L);

		Assert.That(eightyPercent.Value, Is.EqualTo(80d));
		Assert.That(eightyPercent.Fraction, Is.EqualTo(0.8d));

		Percentage<decimal> tenPercent = Percentage<decimal>.FromAmounts(10d, 100d);
		Assert.That(tenPercent.Value, Is.EqualTo(10m));
		Assert.That(tenPercent.Fraction, Is.EqualTo(0.1m));

		Percentage<decimal> thousandPercent = Percentage<decimal>.FromAmounts(100d, 10d);
		Assert.That(thousandPercent.Value, Is.EqualTo(1000m));
		Assert.That(thousandPercent.Fraction, Is.EqualTo(10m));
	}

	[Test]
	public void FromAmount_ZeroTotal_Exception() =>
		Assert.That(() => Percentage<double>.FromAmounts(10, 0), Throws.ArgumentException);

	[Test]
	public void FromDifference_TotalBigger_PositivePercentage()
	{
		Percentage<double> fiftyPercentBigger = Percentage<double>.FromDifference(20L, 10L);
		Assert.That(fiftyPercentBigger.Value, Is.EqualTo(50d));

		fiftyPercentBigger = Percentage<double>.FromDifference(20d, 10d);
		Assert.That(fiftyPercentBigger.Value, Is.EqualTo(50d));
	}

	[Test]
	public void FromDifference_TotalSmaller_NegativePercentage()
	{
		Percentage<decimal> twiceAsSmall = Percentage<decimal>.FromDifference(10L, 20L);
		Assert.That(twiceAsSmall.Value, Is.EqualTo(-100m));

		twiceAsSmall = Percentage<decimal>.FromDifference(10d, 20d);
		Assert.That(twiceAsSmall.Value, Is.EqualTo(-100m));
	}

	[Test]
	public void FromDifference_ZeroTotal_HundredPercent()
	{
		Percentage<float> hundredPercentMore = Percentage<float>.FromDifference(15, 0);
		Assert.That(hundredPercentMore.Value, Is.EqualTo(100f));
		Assert.That(hundredPercentMore.Fraction, Is.EqualTo(1f));

		hundredPercentMore = Percentage<float>.FromDifference(long.MaxValue, 0);
		Assert.That(hundredPercentMore.Value, Is.EqualTo(100d));
		Assert.That(hundredPercentMore.Fraction, Is.EqualTo(1d));
	}

	[Test]
	public void Inference_SaveTyping()
	{
		Percentage<double> fromValue = Percentage.From(45d);
		Assert.That(fromValue.Value, Is.EqualTo(45d));

		Percentage<decimal> fromFraction = Percentage.FromFraction(0.33m);
		Assert.That(fromFraction.Value, Is.EqualTo(33m));

		Percentage<float> fromAmounts = Percentage.FromAmounts<float, short>(25, 200);
		Assert.That(fromAmounts.Value, Is.EqualTo(12.5f));
	}

	[Test]
	public void Extension_RollsOfFingertips()
	{
		Assert.That(.33m.Percent(), Is.EqualTo(Percentage.From(.33m)));

		Assert.That(45d.Percent().Fraction, Is.EqualTo(.45d));
	}
}