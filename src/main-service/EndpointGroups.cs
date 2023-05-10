
public static class EndpointGroups
{
    public static RouteGroupBuilder GroupV1(this RouteGroupBuilder group)
    {
        group.MapGet("/", () => Results.Ok("hello workd"));
        return group;
    }
}