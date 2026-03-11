// --------------------------------------------------------------------------------
// Matrix.cs
// 
// --------------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace SharpVision.Core;

public class Matrix<T> where T : unmanaged
{
    // --------------------------------------------------------------------------------
    // fields and properties
    // --------------------------------------------------------------------------------
    
    public int Rows { get; }
    public int Columns { get; }
    public int Channels { get; }
    public int Stride { get; }

    public T[] Data { get; }

    // --------------------------------------------------------------------------------
    // constructors
    // --------------------------------------------------------------------------------

    public Matrix(int rows, int columns, int channels = 1)
    {
        this.Rows = rows;
        this.Columns = columns;
        this.Channels = channels;
        this.Stride = this.Columns * this.Channels;
        this.Data = new T[this.Rows * this.Stride];
    }

    // --------------------------------------------------------------------------------
    // methods
    // --------------------------------------------------------------------------------

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
