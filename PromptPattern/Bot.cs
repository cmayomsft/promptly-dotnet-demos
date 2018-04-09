using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace PromptPattern
{
    public class BotConversationState
    {
        public string Prompt { get; set; }
    }

    public class BotUserState: StoreItem
    {
        public string Name { get; set; }
    }

    public class Bot : IBot
    {
        public Task OnReceiveActivity(IBotContext context)
        {
            if ((context.Request.Type == ActivityTypes.Message) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                var message = context.Request.AsMessageActivity().Text;

                // If bot doesn't have state it needs, prompt for it.
                if (context.GetUserState<BotUserState>().Name == null)
                {
                    // On the first turn, prompt and update state that conversation is in a prompt.
                    if (context.GetConversationState<BotConversationState>().Prompt != "name")
                    {
                        context.GetConversationState<BotConversationState>().Prompt = "name";
                        context.SendActivity("What is your name?");
                    }
                    else
                    {
                        // On the subsequent turn, update state with reply and update state that prompt has completed.
                        context.GetConversationState<BotConversationState>().Prompt = "";
                        context.GetUserState<BotUserState>().Name = message;
                        context.SendActivity($"Great, I'll call you '{ context.GetUserState<BotUserState>().Name }'!");
                    }
                }
                else
                {
                    context.SendActivity($"{ context.GetUserState<BotUserState>().Name } said: '{ message }'");
                }
            }

            return Task.CompletedTask;
        }
    }
}
