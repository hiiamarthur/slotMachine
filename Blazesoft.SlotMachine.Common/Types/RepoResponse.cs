using System.Diagnostics.CodeAnalysis;

namespace Blazesoft.SlotMachine.Common.Types
{
    public class RepoResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public Exception? Exception { get; set; }
        public static RepoResponse Success() => new() { IsSuccess = true };
        public static RepoResponse Fail(string message, Exception? exception = null) => new() { IsSuccess = false, Message = message, Exception = exception };
    }
    public class RepoResponse<T> : RepoResponse
    {
        [MemberNotNullWhen(true, nameof(Value))]
        internal bool GetSuccessful() => IsSuccess;

        public T? Value { get; set; }
        public static RepoResponse<T> Success(T value) => new() { IsSuccess = true, Value = value };
        public new static RepoResponse<T> Fail(string message, Exception? exception = null) => new() { IsSuccess = false, Message = message, Exception = exception };
    }
}
