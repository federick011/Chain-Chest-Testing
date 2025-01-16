
using System;

[Serializable]
public class SubManagerBase : BaseMananagers
{
    public virtual void InitializeOnAwake(AppManager _appManager) {}

    public virtual void InitializeOnStart(AppManager _appManager){}
}

public interface BaseMananagers
{
    void InitializeOnAwake(AppManager _appManager);
    void InitializeOnStart(AppManager _appManager);
}


