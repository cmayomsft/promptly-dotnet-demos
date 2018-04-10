using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;

namespace PromptPattern
{
    public class BotConversationState
    {
        public string ActivePrompt { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
    }

    public class Bot : IBot
    {
        public Task OnReceiveActivity(IBotContext context)
        {
            if ((context.Request.Type == ActivityTypes.Message) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                var message = context.Request.AsMessageActivity().Text;

                var conversationState = context.GetConversationState<BotConversationState>();

                // On the subsequent turn, update state with reply from user and that prompt has completed.
                if (conversationState.ActivePrompt != null)
                {
                    switch (conversationState.ActivePrompt)
                    {
                        case "namePrompt":
                            conversationState.Name = message;
                            break;

                        case "agePrompt":
                            conversationState.Age = Int32.Parse(message);
                            break;
                    }

                    conversationState.ActivePrompt = null;
                }

                // If bot doesn't have state it needs, prompt for it.
                if (conversationState.Name == null)
                {
                    conversationState.ActivePrompt = "namePrompt";
                    return context.SendActivity("What is your name?");
                }

                if (conversationState.Age == null)
                {
                    conversationState.ActivePrompt = "agePrompt";
                    return context.SendActivity("How old are you?");
                }

                // If the bot has the state it needs, use it!
                return context.SendActivity($"Hello { conversationState.Name }! You are { conversationState.Age } years old.");
            }
            else
            {
                return context.SendActivity($"Received activity of type '{ context.Request.Type }'");
            }
        }
    }
}


                /*
                // On the subsequent turn, update state with reply from user and that prompt has completed.
                if (conversationState.ActivePrompt != null)
                {
                    switch (conversationState.ActivePrompt)
                    {
                        case "namePrompt":
                            userState.Name = message;
                            break;

                        case "agePrompt":
                            userState.Age = Int32.Parse(message);
                            break;
                    }

                    conversationState.ActivePrompt = null;
                }

                // If bot doesn't have state it needs, prompt for it.
                if (userState.Name == null)
                {
                    conversationState.ActivePrompt = "namePrompt";
                    return context.SendActivity("What is your name?");
                }

                if (userState.Age == null)
                {
                    conversationState.ActivePrompt = "agePrompt";
                    return context.SendActivity("How old are you?");
                }

                // If the bot has the state it needs, use it!
                return context.SendActivity($"Hello { userState.Name }! You are { userState.Age } years old.");
                */
