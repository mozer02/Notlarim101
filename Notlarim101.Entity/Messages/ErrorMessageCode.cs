using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.Entity.Messages
{
    public enum ErrorMessageCode
    {
        UsernameAlreadyExist=101,

        EmailalreadyExist=102,
        UserIsNotActive=103,
        UsernameOrPasswordWrong=104,
        CheckYourEmail=105,
        UserAlreadyActive=106,
        ActivateIdDoesNotExist=107,
        UserNotFound=108,
        ProfileCouldNotUpdate=109,
        UserCouldNotRemove=110,
        UserCouldNotFind=111,
        UserCouldNotInserted=112,
        UserCouldNotUpdated=113
    }
}
