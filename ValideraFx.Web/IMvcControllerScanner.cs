using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ValideraFx.Web;

public interface IMvcControllerScanner
{
    Dictionary<string, List<(string Name, Type Type)>> Scan();
}