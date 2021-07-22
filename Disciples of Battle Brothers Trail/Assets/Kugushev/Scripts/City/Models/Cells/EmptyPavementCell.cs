namespace Kugushev.Scripts.City.Models.Cells
{
    public class EmptyPavementCell : CityCell
    {
        public static EmptyPavementCell Instance { get; } = new EmptyPavementCell();

        private EmptyPavementCell()
        {
        }
    }
}