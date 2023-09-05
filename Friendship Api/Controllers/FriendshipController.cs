using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Friendship_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendshipController : Controller
    {

        public static List<string> friendslist = new List<string>
        {
            "Josi,Mario",
            "Paavo,Juha"
        };

        //show list
        [HttpGet("GetAllFriendships")]
        public IActionResult GetAllFriendships()
        {
            return Ok(friendslist);  //status code of 200 (OK) a
        }

        [HttpGet("name")]
        public IActionResult GetFriendsByName(string name)
        {
            var result = friendslist
                .Where(f => f.Contains(name)).ToList();
          
           if(result.Count == 0)
            {
                return NotFound($"No friendship found for {name}");
            }
            else
            {
                return Ok(result);
            }

        }

      //name is in the list to which add new(friendName)name
        // POST api/friendship/{name}
        [HttpPost("{name}")]
        public IActionResult AddFriendToName(string name, [FromBody] string friendName)
        {
            if (string.IsNullOrEmpty(friendName))
            {
                return BadRequest("Friend name cannot be empty.");
            }

            var newFriendship = $"{name},{friendName}";
            if (!friendslist.Contains(newFriendship))
            {
                friendslist.Add(newFriendship);
                return Ok($"Added {friendName} as a friend to {name}");
            }

            return Conflict($"{friendName} is already a friend of {name}");
        }


        [HttpPost("all")]
        public IActionResult AddFriendToAll([FromBody] string friendName)
        {
            if (string.IsNullOrEmpty(friendName))
            {
                return BadRequest("Friend name cannot be empty.");
            }
            // Create a copy of the friendsList to avoid modifying it during iteration
            List<string> copyOfFriendsList = new List<string>(friendslist);
            foreach (var friends in copyOfFriendsList)
            {
                var names = friends.Split(',');
                if (!names.Contains(friendName))
                {
                    friendslist.Add($"{names[0]},{friendName}");
                    friendslist.Add($"{friendName},{names[1]}");
                }
            }

            return Ok($"Added {friendName} as a friend to all names.");
        }


        [HttpDelete("RemovefromAll")]
        public IActionResult RemoveNameFromAllFriends(string name)
        {
            friendslist.RemoveAll(friends => friends.Contains(name));
            return Ok($"Removed {name} from all friendships.");
        }


        [HttpDelete("RemoveFriendship")]
        public IActionResult RemoveFriednship ([FromBody] string friendship)
        {
            if (string.IsNullOrEmpty(friendship))
            {
                return BadRequest("Invalid friendship data.");
            }
            if (friendslist.Contains(friendship))
            {
                friendslist.Remove(friendship);
                    return Ok("Successfully removed");
            }
            else
            {
               return BadRequest("Friendship does not exist");
            }
            
        }


    }
}

