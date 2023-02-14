using System;
using System.Buffers;

namespace Woodstar.Tds;

static class BackendMessage
{
    public static readonly bool DebugEnabled = false;
}

enum ReadStatus
{
    Done,
    ConsumeData,
    NeedMoreData,
    InvalidData,
    AsyncResponse
}

static class OperationStatusExtensions
{
    static ReadStatus ThrowArgumentOutOfRange(OperationStatus status) => throw new ArgumentOutOfRangeException(nameof(status), status, null);

    public static ReadStatus ToReadStatus(this OperationStatus status)
        => status switch
        {
            OperationStatus.Done => ReadStatus.Done,
            OperationStatus.NeedMoreData => ReadStatus.NeedMoreData,
            OperationStatus.InvalidData => ReadStatus.InvalidData,
            _ => ThrowArgumentOutOfRange(status)
        };
}

interface IBackendMessage<THeader> where THeader : struct, IHeader<THeader>
{
    ReadStatus Read(ref MessageReader<THeader> reader);
}
