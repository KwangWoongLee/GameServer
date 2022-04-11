using ServerCore;
using System;

class PacketHandler
{
    public static void C_PlayerInfoReqHandler(PacketSession session, IPacket packet)
    {
        C_PlayerInfoReq p = packet as C_PlayerInfoReq;
        Console.WriteLine($"PlayerInfoReq : {p.playerId} , name : {p.name}");

        foreach (var skill in p.skills)
        {
            Console.WriteLine($"Skill id : {skill.id}, level : {skill.level}, duration : {skill.duration}");
        }
    }
}
