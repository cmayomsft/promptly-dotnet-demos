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
        public Task OnTurn(ITurnContext turnContext)
        {
            if ((turnContext.Activity.Type == ActivityTypes.Message) && (turnContext.Activity.Text.Length > 0))
            {
                var message = turnContext.Activity.Text;

                var conversationState = turnContext.GetConversationState<BotConversationState>();

                // On the subsequent turn, update state with reply from user and that prompt has completed.


                // If bot doesn't have state it needs, prompt for it.


                // If the bot has the state it needs, use it!
                return turnContext.SendActivity($"Hello { conversationState.Name }! You are { conversationState.Age } years old.");
            }
            else
            {
                return turnContext.SendActivity($"Received activity of type '{ turnContext.Activity.Type }'");
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
                    return turnContext.SendActivity("What is your name?");
                }

                if (conversationState.Age == null)
                {
                    conversationState.ActivePrompt = "agePrompt";
                    return turnContext.SendActivity("How old are you?");
                }
                */
