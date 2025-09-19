using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apiblokes.Game.Model;

namespace Apiblokes.Game.Managers.Battle;

public static class BattleFlavor
{
    private static Dictionary<BlokeType, Dictionary<BlokeType, string>> SuccessfulAttackText = new();
    private static Dictionary<BlokeType, Dictionary<BlokeType, string>> FailedAttackText = new();

    static BattleFlavor()
    {
        SetupSuccessfulAttackText();
        SetupFailedAttackText();
    }

    private static void SetupFailedAttackText()
    {
        FailedAttackText[BlokeType.Manager] = new Dictionary<BlokeType, string>();
        FailedAttackText[BlokeType.Manager][BlokeType.Manager] = "{0} replaces {1}'s PowerPoint with cat pictures, but the director actually loves it.";
        FailedAttackText[BlokeType.Manager][BlokeType.Network] = "{0} tries to reduce {1}'s budget, but gives up when the WIFI goes out.";
        FailedAttackText[BlokeType.Manager][BlokeType.SystemAdmin] = "{0} tries to reduce {1}'s budget, but gives up on this joke.";
        FailedAttackText[BlokeType.Manager][BlokeType.HelpDesk] = "{0} tries to reduce {1}'s budget, but gives up when forced to use Windows.";
        FailedAttackText[BlokeType.Manager][BlokeType.Developer] = "{0} tries to reduce {1}'s budget, but developers don't have a budget.";
        FailedAttackText[BlokeType.Manager][BlokeType.DoItAll] = "{0} tries to slash {1}s budget, but gives up after finding their tires slashed.";

        FailedAttackText[BlokeType.Network] = new Dictionary<BlokeType, string>();
        FailedAttackText[BlokeType.Network][BlokeType.Manager] = "{0} disconnects the WIFI, but {1} had fiber installed in their office a year ago.";
        FailedAttackText[BlokeType.Network][BlokeType.Network] = "{0} tries to cause havoc, but {1}'s network is perfectly configured.";
        FailedAttackText[BlokeType.Network][BlokeType.SystemAdmin] = "{0} disconnect's the servers, but {1} has backups.";
        FailedAttackText[BlokeType.Network][BlokeType.HelpDesk] = "{0} disconnects the WIFI, but {1} hardwired all the computers.";
        FailedAttackText[BlokeType.Network][BlokeType.Developer] = "{0} blocks common ports, but {1} writes a proxy.";
        FailedAttackText[BlokeType.Network][BlokeType.DoItAll] = "{0} tries to turn off {1}'s internet, but gives up after their tires slashed.";

        FailedAttackText[BlokeType.SystemAdmin] = new Dictionary<BlokeType, string>();
        FailedAttackText[BlokeType.SystemAdmin][BlokeType.Manager] = "{0} disables system access, but {1} doesn't notice.";
        FailedAttackText[BlokeType.SystemAdmin][BlokeType.Network] = "{0} disables the mail server, but {1} reroutes to the cloud.";
        FailedAttackText[BlokeType.SystemAdmin][BlokeType.SystemAdmin] = "{0} attacks with SharePoint, but {1} actually likes SharePoint.";
        FailedAttackText[BlokeType.SystemAdmin][BlokeType.HelpDesk] = "{0} breaks Windows configuration, but {1} already migrated everyone to Chromebooks.";
        FailedAttackText[BlokeType.SystemAdmin][BlokeType.Developer] = "{0} disables web server, but {1} has already migrated to AWS.";
        FailedAttackText[BlokeType.SystemAdmin][BlokeType.DoItAll] = "{0} tries to disable {1}'s systems, but gives up after finding their tires slashed.";

        FailedAttackText[BlokeType.HelpDesk] = new Dictionary<BlokeType, string>();
        FailedAttackText[BlokeType.HelpDesk][BlokeType.Manager] = "{0} remotes into {1}'s computer, but {1} keeps moving the mouse.";
        FailedAttackText[BlokeType.HelpDesk][BlokeType.Network] = "{0} remotes into {1}'s computer, but {1} turns off the WIFI.";
        FailedAttackText[BlokeType.HelpDesk][BlokeType.SystemAdmin] = "{0} remotes into {1}'s computer, but its actually a honey pot.";
        FailedAttackText[BlokeType.HelpDesk][BlokeType.HelpDesk] = "{0} remotes into {1}'s computer, but {1} keeps moving the mouse.";
        FailedAttackText[BlokeType.HelpDesk][BlokeType.Developer] = "{0} remotes into {1}'s computer, but it's Linux.";
        FailedAttackText[BlokeType.HelpDesk][BlokeType.DoItAll] = "{0} remotes into {1}'s computer, but gives up after finding their tires slashed.";

        FailedAttackText[BlokeType.Developer] = new Dictionary<BlokeType, string>();
        FailedAttackText[BlokeType.Developer][BlokeType.Manager] = "{0} asks {1} for a raise, but is threatened to be replaced by AI";
        FailedAttackText[BlokeType.Developer][BlokeType.Network] = "{0} DDOSes {1}'s network, but the firewall stops it.";
        FailedAttackText[BlokeType.Developer][BlokeType.SystemAdmin] = "{0} tries to move {1}'s servers to the cloud, but still can't configure a VM";
        FailedAttackText[BlokeType.Developer][BlokeType.HelpDesk] = "{0} tries to show off IQ to {1}, but can't get two monitors to work.";
        FailedAttackText[BlokeType.Developer][BlokeType.Developer] = "{0} tries to explain functional programming, but {1} has actual work to do.";
        FailedAttackText[BlokeType.Developer][BlokeType.DoItAll] = "{0} attempts to hack {1}'s website, but gives up after finding their tires slashed.";

        FailedAttackText[BlokeType.DoItAll] = new Dictionary<BlokeType, string>();
        FailedAttackText[BlokeType.DoItAll][BlokeType.Manager] = "{0} tries to send {1} the bill, but is too busy.";
        FailedAttackText[BlokeType.DoItAll][BlokeType.Network] = "{0} tries to hack {1}'s WIFI password, but is too busy.";
        FailedAttackText[BlokeType.DoItAll][BlokeType.SystemAdmin] = "{0} tries to teach {1} about Linux, but is too busy.";
        FailedAttackText[BlokeType.DoItAll][BlokeType.HelpDesk] = "{0} tries to replace {1}'s ticketing software with a text file, but is too busy.";
        FailedAttackText[BlokeType.DoItAll][BlokeType.Developer] = "{0} tries to use AI to replace {1}'s code, but is too busy.";
        FailedAttackText[BlokeType.DoItAll][BlokeType.DoItAll] = "{0} tries to out busy {1}, but is too busy.";
    }

    private static void SetupSuccessfulAttackText()
    {
        SuccessfulAttackText[BlokeType.Manager] = new Dictionary<BlokeType, string>();
        SuccessfulAttackText[BlokeType.Manager][BlokeType.Manager] = "{0} takes the director golfing leaving {1} wondering what they are talking about.";
        SuccessfulAttackText[BlokeType.Manager][BlokeType.Network] = "{0} asks {1} to set up their home router.";
        SuccessfulAttackText[BlokeType.Manager][BlokeType.SystemAdmin] = "{0} slashes {1}'s budget in half.";
        SuccessfulAttackText[BlokeType.Manager][BlokeType.HelpDesk] = "{0} schedules {1} to replace all the printers overnight.";
        SuccessfulAttackText[BlokeType.Manager][BlokeType.Developer] = "{0} schedules more meetings to ask {1} why the project isn't done yet.";
        SuccessfulAttackText[BlokeType.Manager][BlokeType.DoItAll] = "{0} adds more to {1}'s workload.";

        SuccessfulAttackText[BlokeType.Network] = new Dictionary<BlokeType, string>();
        SuccessfulAttackText[BlokeType.Network][BlokeType.Manager] = "{0} sends {1} the bill.";
        SuccessfulAttackText[BlokeType.Network][BlokeType.Network] = "{0} adds a loopback in {1}'s network.";
        SuccessfulAttackText[BlokeType.Network][BlokeType.SystemAdmin] = "{0} cuts {1}'s bandwidth in half.";
        SuccessfulAttackText[BlokeType.Network][BlokeType.HelpDesk] = "{0} has {1} crawl through ducting to run new Cat5";
        SuccessfulAttackText[BlokeType.Network][BlokeType.Developer] = "{0} swears it's not DNS. {1} eliminates all other possibilities before discovering it was DNS.";
        SuccessfulAttackText[BlokeType.Network][BlokeType.DoItAll] = "{0} complains to {1} about their workload.";

        SuccessfulAttackText[BlokeType.SystemAdmin] = new Dictionary<BlokeType, string>();
        SuccessfulAttackText[BlokeType.SystemAdmin][BlokeType.Manager] = "{0} sends {1} the bill.";
        SuccessfulAttackText[BlokeType.SystemAdmin][BlokeType.Network] = "{0} switches servers to different vlans with telling {1}.";
        SuccessfulAttackText[BlokeType.SystemAdmin][BlokeType.SystemAdmin] = "{0} tells {1} about Arch Linux.";
        SuccessfulAttackText[BlokeType.SystemAdmin][BlokeType.HelpDesk] = "{0} has {1} manually update a setting on every computer.";
        SuccessfulAttackText[BlokeType.SystemAdmin][BlokeType.Developer] = "{0} locks {1} out of the application server.";
        SuccessfulAttackText[BlokeType.SystemAdmin][BlokeType.DoItAll] = "{0} complains to {1} about their workload.";

        SuccessfulAttackText[BlokeType.HelpDesk] = new Dictionary<BlokeType, string>();
        SuccessfulAttackText[BlokeType.HelpDesk][BlokeType.Manager] = "{0} sends {1} the bill.";
        SuccessfulAttackText[BlokeType.HelpDesk][BlokeType.Network] = "{0} escalates all WIFI issues to {1}.";
        SuccessfulAttackText[BlokeType.HelpDesk][BlokeType.SystemAdmin] = "{0} optimizes computers by disabling {1}'s security solutions.";
        SuccessfulAttackText[BlokeType.HelpDesk][BlokeType.HelpDesk] = "{0} stole the good screwdriver from {1}.";
        SuccessfulAttackText[BlokeType.HelpDesk][BlokeType.Developer] = "{0} asks {1} to fix vendor products.";
        SuccessfulAttackText[BlokeType.HelpDesk][BlokeType.DoItAll] = "{0} complains to {1} about their workload.";

        SuccessfulAttackText[BlokeType.Developer] = new Dictionary<BlokeType, string>();
        SuccessfulAttackText[BlokeType.Developer][BlokeType.Manager] = "{0} asks {1} for a raise.";
        SuccessfulAttackText[BlokeType.Developer][BlokeType.Network] = "{0} accidentally DDOSes {1}'s network.";
        SuccessfulAttackText[BlokeType.Developer][BlokeType.SystemAdmin] = "{0} moves services from {1}'s servers to the cloud.";
        SuccessfulAttackText[BlokeType.Developer][BlokeType.HelpDesk] = "{0} has {1} support their buggy code.";
        SuccessfulAttackText[BlokeType.Developer][BlokeType.Developer] = "{0} tells {1} to rewrite their code in Rust.";
        SuccessfulAttackText[BlokeType.Developer][BlokeType.DoItAll] = "{0} complains to {1} about their workload.";

        SuccessfulAttackText[BlokeType.DoItAll] = new Dictionary<BlokeType, string>();
        SuccessfulAttackText[BlokeType.DoItAll][BlokeType.Manager] = "{0} sends {1} the bill and asks for a raise.";
        SuccessfulAttackText[BlokeType.DoItAll][BlokeType.Network] = "{0} schools {1} on networking.";
        SuccessfulAttackText[BlokeType.DoItAll][BlokeType.SystemAdmin] = "{0} schools {1} on administration.";
        SuccessfulAttackText[BlokeType.DoItAll][BlokeType.HelpDesk] = "{0} schools {1} on support.";
        SuccessfulAttackText[BlokeType.DoItAll][BlokeType.Developer] = "{0} replaces {1}'s months of work with a small script.";
        SuccessfulAttackText[BlokeType.DoItAll][BlokeType.DoItAll] = "{0} complains to {1} about their workload.";
    }

    public static string SuccessfulAttack( BlokeType attacker, BlokeType target )
    {
        return SuccessfulAttackText[attacker][target];
    }

    public static string FailedAttack( BlokeType attacker, BlokeType target )
    {
        return FailedAttackText[attacker][target];
    }
}
