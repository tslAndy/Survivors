namespace Components.Basic;

[System.Runtime.CompilerServices.InlineArray(8)]
struct DispComp
{
    private IDisposable? disp;

    public DispComp() { }

    public DispComp(IDisposable elem_0) => this[0] = elem_0;

    public DispComp(IDisposable elem_0, IDisposable elem_1)
    {
        this[0] = elem_0;
        this[1] = elem_1;
    }

    public DispComp(IDisposable elem_0, IDisposable elem_1, IDisposable elem_2)
    {
        this[0] = elem_0;
        this[1] = elem_1;
        this[2] = elem_2;
    }

    public DispComp(IDisposable elem_0, IDisposable elem_1, IDisposable elem_2, IDisposable elem_3)
    {
        this[0] = elem_0;
        this[1] = elem_1;
        this[2] = elem_2;
        this[3] = elem_3;
    }

    public DispComp(
        IDisposable elem_0,
        IDisposable elem_1,
        IDisposable elem_2,
        IDisposable elem_3,
        IDisposable elem_4
    )
    {
        this[0] = elem_0;
        this[1] = elem_1;
        this[2] = elem_2;
        this[3] = elem_3;
        this[4] = elem_4;
    }

    public DispComp(
        IDisposable elem_0,
        IDisposable elem_1,
        IDisposable elem_2,
        IDisposable elem_3,
        IDisposable elem_4,
        IDisposable elem_5
    )
    {
        this[0] = elem_0;
        this[1] = elem_1;
        this[2] = elem_2;
        this[3] = elem_3;
        this[4] = elem_4;
        this[5] = elem_5;
    }

    public DispComp(
        IDisposable elem_0,
        IDisposable elem_1,
        IDisposable elem_2,
        IDisposable elem_3,
        IDisposable elem_4,
        IDisposable elem_5,
        IDisposable elem_6
    )
    {
        this[0] = elem_0;
        this[1] = elem_1;
        this[2] = elem_2;
        this[3] = elem_3;
        this[4] = elem_4;
        this[5] = elem_5;
        this[6] = elem_6;
    }

    public DispComp(
        IDisposable elem_0,
        IDisposable elem_1,
        IDisposable elem_2,
        IDisposable elem_3,
        IDisposable elem_4,
        IDisposable elem_5,
        IDisposable elem_6,
        IDisposable elem_7
    )
    {
        this[0] = elem_0;
        this[1] = elem_1;
        this[2] = elem_2;
        this[3] = elem_3;
        this[4] = elem_4;
        this[5] = elem_5;
        this[6] = elem_6;
        this[7] = elem_7;
    }
}
