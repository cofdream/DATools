namespace DATools
{
    public interface IDevelopementTool
    {
        string ToolName { get; }
        void Init();
        void Dispose();
        void Enable();
        void Disable();
        void OnGUI();
    }
}