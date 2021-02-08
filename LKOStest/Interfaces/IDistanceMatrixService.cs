namespace LKOStest.Services
{
    public interface IDistanceMatrixService
    {
        public DistanceCalculation GetDistance(string origin, string destination);
    }
}