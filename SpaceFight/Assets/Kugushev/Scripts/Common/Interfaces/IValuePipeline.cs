namespace Kugushev.Scripts.Common.Interfaces
{
    public interface IValuePipeline<in T>
    {
        float Calculate(float value, T criteria);
    }
}