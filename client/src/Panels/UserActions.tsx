import { useAuth0 } from "@auth0/auth0-react"
import StandardButton from "../Components/StandardButton"
import Page from "../Enums/Page"
import Action, { ActionType } from "../Reducer/Action"
import AppState from "../Reducer/AppState"
import classes from './UserActions.module.scss'

type UserActionsProps = {
    state: AppState,
    dispatch: React.Dispatch<Action>
}

const UserActions = ({ state, dispatch }: UserActionsProps) => {
    const { user, logout } = useAuth0()
    const onLibraryClick = () => dispatch({ type: ActionType.SELECTED_PAGE_UPDATED, updatedPage: Page.Library })

    return (
        <div className={`d-flex flex-column justify-content-center p-2 ${classes.background} ${classes.shadow}`}>
            {user && <h4 className="text-center">{user.given_name}</h4>}
            <StandardButton className="m-2" iconClasses="fa-solid fa-list py-2" toolTipText="Library" isFixedSize={true} onClick={onLibraryClick} />
            <StandardButton className="m-2" iconClasses="fa-solid fa-door-open py-2" toolTipText="Logout" isFixedSize={true} onClick={() => logout({ returnTo: window.location.origin })} />
        </div>
    )
}

export default UserActions