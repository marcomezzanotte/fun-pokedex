using System.Threading.Tasks;

namespace FunPokedex.Core.Interfaces;

public interface ITextTranslator
{
    Task<string> ApplyShakespereanTranslationAsync(string value);
    Task<string> ApplyYodaTranslationAsync(string value);
}
