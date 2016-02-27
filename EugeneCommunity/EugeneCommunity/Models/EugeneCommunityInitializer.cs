using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EugeneCommunity.Models
{
    public class EugeneCommunityInitializer : DropCreateDatabaseAlways<EugeneCommunityContext>
    {
        /*
        protected override void Seed(EugeneCommunityContext context)
        {
            Topic t1 = new Topic { TopicId = 1, Title = "IPA Crave" };
            Topic t2 = new Topic { TopicId = 2, Title = "Beer Tasting" };
            context.Topics.Add(t1);
            context.Topics.Add(t2);
            DateTime dt1 = new DateTime(2016, 2, 8, 13, 30, 00);
            DateTime dt2 = new DateTime(2016, 2, 7, 11, 30, 00);
            DateTime dt3 = new DateTime(2016, 2, 6, 16, 30, 00);
            DateTime dt4 = new DateTime(2016, 2, 5, 20, 30, 00);

            Member m1 = new Member { MemberId = 1, UserName = "Brody", Email = "brodyjcase@gmail.com", Password = "Passw0rd", IsAdmin = true};
            Member m2 = new Member { MemberId = 2, UserName = "Zach", Email = "beerlove@gmail.com", Password = "Passw0rd", IsAdmin = false };
            context.Members.Add(m1);
            context.Members.Add(m2);

            context.Messages.Add(new Message { 
                MessageId = 1,
                TopicId = 1,
                Body = "HUB (Hopworks Urban Brewery) makes my favorite IPA as of now. They source a single hop called the Simcoe, it's grown in Washington. Not to bitter, but a smooth, crisp flavor jumps out at you. I love all the organic beer that HUB crafts. Give it a try!",
                Date = dt1,
                MemberId = 1
            });

            context.Messages.Add(new Message
            {
                MessageId = 2,
                TopicId = 2,
                Body = "With so many pourhouses here in Eugene, where do you even start?! Well, I'll give you a shortened list to tackle: Falling Sky Delicatessen, The Bier Stein, The Tap and Growler. Pick one and have fun tasting!",
                Date = dt2,
                MemberId = 2
            });

            context.Messages.Add(new Message
            {
                MessageId = 3,
                TopicId = 1,
                Body = "While typically a Summer seasonal beer, Pelican Brewery currently is putting out their 'Umbrella' IPA. It's supreme. Although, pelicans really are nasty birds.",
                Date = dt3,
                MemberId = 2
            });

            context.Messages.Add(new Message
            {
                MessageId = 3,
                TopicId = 2,
                Body = "Another great taphouse is 16 Tons. They're location off Willamette St (next to Market of Chores) is also a cafe. What goes better than anything else with beer? In my opinion, crepes! And 16 Tons has those! So, go pair a porter with a sweet crepe for something new.",
                Date = dt4,
                MemberId = 1
            });

            base.Seed(context);
        }
         * */
    }
}