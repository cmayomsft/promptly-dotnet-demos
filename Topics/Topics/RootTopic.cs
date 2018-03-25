using Topics.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using PromptlyBot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Topics.Topics
{
    public class RootTopicState : ConversationTopicState
    {

    }

    public class RootTopic : TopicsRoot<BotConversationState, RootTopicState>
    {
        private const string ADD_ALARM_TOPIC = "addAlarmTopic";

        public RootTopic(IBotContext context) : base(context)
        {
            // User state initialization should be done once in the welcome 
            //  new user feature. Placing it here until that feature is added.
            if (context.GetUserState<BotUserState>().Alarms == null)
            {
                context.GetUserState<BotUserState>().Alarms = new List<Alarm>();
            }

            this.SubTopics.Add(ADD_ALARM_TOPIC, (object[] args) =>
            {
                var addAlarmTopic = new AddAlarmTopic();

                addAlarmTopic.Set
                    .OnSuccess((ctx, alarm) =>
                        {
                            this.ClearActiveTopic();

                            ctx.GetUserState<BotUserState>().Alarms.Add(alarm);

                            context.SendActivity($"Added alarm named '{ alarm.Title }' set for '{ alarm.Time }'.");
                        })
                    .OnFailure((ctx, reason) =>
                        {
                            this.ClearActiveTopic();

                            context.SendActivity("Let's try something else.");
                            
                            this.ShowDefaultMessage(ctx);
                        });

                return addAlarmTopic;
            });
        }

        public override Task OnReceiveActivity(IBotContext context)
        {
            if ((context.Request.Type == ActivityTypes.Message) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                var message = context.Request.AsMessageActivity();

                // If the user wants to change the topic of conversation...
                if (message.Text.ToLowerInvariant() == "add alarm")
                {
                    // Set the active topic and let the active topic handle this turn.
                    this.SetActiveTopic(ADD_ALARM_TOPIC)
                            .OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                // If there is an active topic, let it handle this turn until it completes.
                if (HasActiveTopic)
                {
                    ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                ShowDefaultMessage(context);
            }

            return Task.CompletedTask;
        }

        private void ShowDefaultMessage(IBotContext context)
        {
            context.SendActivity("'Add Alarm'.");
        }
    }
}
