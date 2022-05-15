import { useAuth0 } from "@auth0/auth0-react"
import StandardButton, { ButtonSize } from "../../Components/StandardButton"
import PageEnum from "../../Enums/PageEnum"
import Action, { ActionType } from "../../Reducer/Action"
import classes from './UserActions.module.scss'

type UserActionsProps = {
    dispatch: React.Dispatch<Action>
}

const UserActions = ({ dispatch }: UserActionsProps) => {
    const { user, logout } = useAuth0()
    const onLibraryClick = () => dispatch({ type: ActionType.SELECTED_PAGE_UPDATED, updatedPage: PageEnum.Library })

    return (
        <div className={`d-flex flex-column justify-content-center p-2 ${classes.background} ${classes.shadow}`}>
            {user && <h4 className="text-center">{user.given_name}</h4>}
            <StandardButton className="m-2" iconClasses="fa-solid fa-list py-2" toolTipText="Library" size={ButtonSize.LARGE} onClick={onLibraryClick} />
            <StandardButton className="m-2" iconClasses="fa-solid fa-door-open py-2" toolTipText="Logout" size={ButtonSize.LARGE} onClick={() => logout({ returnTo: window.location.origin })} />
        </div>
    )
}

export default UserActions