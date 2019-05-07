using System;

public abstract class Controller<T> :IDisposable where T:IModel, new() {
    protected T m_data;

    public Controller() {
        m_data = new T();
        m_data.Init();
        Init();
    }

    protected abstract void Init();

    public void Dispose() {
        m_data.Dispose();
    }
}
