namespace Web.Services
{
    public class AiDecisionService
    {
        public bool ShouldUseAI(int confidence)
        {
            return confidence < 50;
        }
    }
}
