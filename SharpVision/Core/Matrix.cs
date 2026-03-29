// --------------------------------------------------------------------------------
// Matrix.cs
// --------------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Buffers;

namespace SharpVision;

public class Matrix<T> : IDisposable where T : unmanaged
{
    // --------------------------------------------------------------------------------
    // fields and properties
    // --------------------------------------------------------------------------------

    public int Rows { get; private set; }
    public int Columns { get; private set; }
    public int Channels { get; private set; }
    public int Stride { get; private set; }
    public int Length { get; private set; }

    public T[] Data { get; private set; } = Array.Empty<T>();

    // --------------------------------------------------------------------------------
    // constructors
    // --------------------------------------------------------------------------------

    public Matrix()
    {
    }

    public Matrix(int rows, int columns, int channels = 1)
    {
        ReallocateIfNeeded(rows, columns, channels);
    }

    // --------------------------------------------------------------------------------
    // methods
    // --------------------------------------------------------------------------------

    public void ReallocateIfNeeded(int rows, int columns, int channels)
    {
        this.Rows = rows;
        this.Columns = columns;
        this.Channels = channels;
        this.Stride = columns * channels;
        this.Length = rows * columns * channels;

        int requiredSize = rows * columns * channels;

        // if we already have an array that's big enough, keep using
        if (this.Data is not null && this.Data.Length >= requiredSize)
            return;

        // if current array too small, return to pool first
        if (this.Data is not null && this.Data.Length > 0)
            ArrayPool<T>.Shared.Return(this.Data);

        // rent new array
        this.Data = ArrayPool<T>.Shared.Rent(requiredSize);
    }

    public void Dispose()
    {
        if (this.Data.Length > 0)
            ArrayPool<T>.Shared.Return(this.Data);
        this.Data = Array.Empty<T>();
        this.Rows = 0;
        this.Columns = 0;
        this.Channels = 0;
    }

    public override string ToString()
    {
        return $"({Rows}r x {Columns}c x {Channels}) [{typeof(T).Name}]";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetPixel(int row, int col, int channel = 0)
    {
        return this.Data[(row * this.Stride) + (col * this.Channels) + channel];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetPixel(int row, int col, int channel, T value)
    {
        this.Data[(row * this.Stride) + (col * this.Channels) + channel] = value;
    }
}
