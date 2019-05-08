using System;

public abstract class Model:IDisposable {
    public Model() {
        Init();
    }
    public abstract void Init();
    public abstract void Dispose();
}
