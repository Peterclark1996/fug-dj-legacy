import { useAuth0 } from "@auth0/auth0-react"
import StandardButton from "../Components/StandardButton"
import classes from './UserActions.module.scss'

const UserActions = () => {
    const { user, logout } = useAuth0()

    return (
        <div className={`d-flex flex-column justify-content-center p-2 ${classes.background} ${classes.shadow}`}>
            {user && <h4 className="text-center">{user.given_name}</h4>}
            <StandardButton className="m-2" iconClasses="fa-solid fa-arrow-right-from-bracket py-2" toolTipText="Logout" onClick={() => logout({ returnTo: window.location.origin })} />
        </div>
    )
}

export default UserActions