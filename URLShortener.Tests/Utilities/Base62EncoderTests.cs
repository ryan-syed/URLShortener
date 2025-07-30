using URLShortener.Utilities;
using FluentAssertions;

namespace URLShortener.Tests.Utilities;

[TestClass]
public class Base62EncoderTests
{
    private Base62Encoder _encoder = null!;

    [TestInitialize]
    public void Setup()
    {
        _encoder = new Base62Encoder();
    }

    [TestMethod]
    public void Encode_WithZero_ReturnsZero()
    {
        // Arrange
        var input = 0.0;

        // Act
        var result = _encoder.Encode(input);

        // Assert
        result.Should().Be("0");
    }

    [DataTestMethod]
    [DataRow(0.5)]
    [DataRow(0.25)]
    [DataRow(0.75)]
    [DataRow(1.0)]
    public void Encode_WithValidInput_ReturnsValidBase62String(double input)
    {
        // Act
        var result = _encoder.Encode(input);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().MatchRegex("^[0-9A-Za-z]+$", "result should only contain Base62 characters");
    }

    [TestMethod]
    public void Encode_WithSameInput_ReturnsConsistentOutput()
    {
        // Arrange
        var input = 0.42;

        // Act
        var result1 = _encoder.Encode(input);
        var result2 = _encoder.Encode(input);

        // Assert
        result1.Should().Be(result2);
    }

    [DataTestMethod]
    [DataRow(0.1)]
    [DataRow(0.5)]
    [DataRow(0.9)]
    public void Encode_OutputLength_IsReasonableForBase62Encoding(double input)
    {
        // Act
        var result = _encoder.Encode(input);

        // Assert
        result.Length.Should().BeGreaterThan(0);
        // Base62 encoding of (double * long.MaxValue) can produce longer strings
        result.Length.Should().BeLessOrEqualTo(30); // Reasonable upper bound for full Base62 encoded values
    }

    [TestMethod]
    public void Encode_WithDifferentInputs_ReturnsDifferentOutputs()
    {
        // Arrange
        var input1 = 0.1;
        var input2 = 0.9;

        // Act
        var result1 = _encoder.Encode(input1);
        var result2 = _encoder.Encode(input2);

        // Assert
        result1.Should().NotBe(result2);
    }

    [DataTestMethod]
    [DataRow(0.0)]
    [DataRow(0.1)]
    [DataRow(0.5)]
    [DataRow(0.9)]
    [DataRow(1.0)]
    public void Encode_CharacterSet_ContainsOnlyValidBase62Characters(double input)
    {
        // Arrange
        var validChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        // Act
        var result = _encoder.Encode(input);

        // Assert
        foreach (char c in result)
        {
            validChars.Should().Contain(c.ToString(), $"'{c}' should be a valid Base62 character");
        }
    }

}