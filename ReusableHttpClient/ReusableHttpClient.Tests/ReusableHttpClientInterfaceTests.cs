namespace ReusableHttpClient.Tests;

public class ReusableHttpClientInterfaceTests
{
    private readonly IReusableHttpClient _reusableHttpClient;

    public ReusableHttpClientInterfaceTests()
    {
        _reusableHttpClient = Substitute.For<IReusableHttpClient>();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnResult()
    {
        // Arrange
        TestResponse expectedResult = new TestResponse { Id = 1, Name = "Test" };
        _reusableHttpClient.GetAsync<TestResponse>("api/test", Arg.Any<CancellationToken>())!.Returns(Task.FromResult(expectedResult));

        // Act
        TestResponse? result = await _reusableHttpClient.GetAsync<TestResponse>("api/test");

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task PostAsync_ShouldReturnString()
    {
        // Arrange
        TestRequest payload = new TestRequest { Name = "Test" };
        string expectedResult = "Success";
        _reusableHttpClient.PostAsync("api/test", payload, Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedResult));

        // Act
        string result = await _reusableHttpClient.PostAsync("api/test", payload);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task PostAsync_WithResponse_ShouldReturnResult()
    {
        // Arrange
        TestRequest payload = new TestRequest { Name = "Test" };
        TestResponse expectedResult = new TestResponse { Id = 1, Name = "Test" };
        _reusableHttpClient.PostAsync<TestRequest, TestResponse>("api/test", payload, Arg.Any<CancellationToken>())!.Returns(
            Task.FromResult(expectedResult));

        // Act
        TestResponse? result = await _reusableHttpClient.PostAsync<TestRequest, TestResponse>("api/test", payload);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnString()
    {
        // Arrange
        TestRequest payload = new TestRequest { Name = "Test" };
        string expectedResult = "Success";
        _reusableHttpClient.PatchAsync("api/test", payload, Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedResult));

        // Act
        string result = await _reusableHttpClient.PatchAsync("api/test", payload);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task PatchAsync_WithResponse_ShouldReturnResult()
    {
        // Arrange
        TestRequest payload = new TestRequest { Name = "Test" };
        TestResponse expectedResult = new TestResponse { Id = 1, Name = "Test" };
        _reusableHttpClient.PatchAsync<TestRequest, TestResponse>("api/test", payload, Arg.Any<CancellationToken>())!.Returns(
            Task.FromResult(expectedResult));

        // Act
        TestResponse? result = await _reusableHttpClient.PatchAsync<TestRequest, TestResponse>("api/test", payload);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task PutAsync_ShouldReturnString()
    {
        // Arrange
        TestRequest payload = new TestRequest { Name = "Test" };
        string expectedResult = "Success";
        _reusableHttpClient.PutAsync("api/test", payload, Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedResult));

        // Act
        string result = await _reusableHttpClient.PutAsync("api/test", payload);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task PutAsync_WithResponse_ShouldReturnResult()
    {
        // Arrange
        TestRequest payload = new TestRequest { Name = "Test" };
        TestResponse expectedResult = new TestResponse { Id = 1, Name = "Test" };
        _reusableHttpClient.PutAsync<TestRequest, TestResponse>("api/test", payload, Arg.Any<CancellationToken>())!.Returns(
            Task.FromResult(expectedResult));

        // Act
        TestResponse? result = await _reusableHttpClient.PutAsync<TestRequest, TestResponse>("api/test", payload);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnString()
    {
        // Arrange
        string expectedResult = "Success";
        _reusableHttpClient.DeleteAsync("api/test", Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedResult));

        // Act
        string result = await _reusableHttpClient.DeleteAsync("api/test");

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task DeleteAsync_WithPayload_ShouldReturnString()
    {
        // Arrange
        TestRequest payload = new TestRequest { Name = "Test" };
        string expectedResult = "Success";
        _reusableHttpClient.DeleteAsync("api/test", payload, Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedResult));

        // Act
        string result = await _reusableHttpClient.DeleteAsync("api/test", payload);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void ClearAuthorizationHeader_ShouldClearHeader()
    {
        // Arrange
        string scheme = "Bearer";

        // Act
        _reusableHttpClient.ClearAuthorizationHeader(scheme);

        // Assert
        _reusableHttpClient.Received(1).ClearAuthorizationHeader(scheme);
    }

    [Fact]
    public void SetAuthorizationHeader_ShouldSetHeader()
    {
        // Arrange
        string scheme = "Bearer";
        string value = "token";

        // Act
        _reusableHttpClient.SetAuthorizationHeader(scheme, value);

        // Assert
        _reusableHttpClient.Received(1).SetAuthorizationHeader(scheme, value);
    }
}