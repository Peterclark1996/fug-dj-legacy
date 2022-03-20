import { useAuth0 } from "@auth0/auth0-react"
import Logo from "../Components/Logo"
import StandardButton from "../Components/StandardButton"
import Title from "../Components/Title"
import classes from './Screen.module.scss'

const LoginScreen = () => {
    const { loginWithRedirect } = useAuth0()

    return (
        <div className={`d-flex flex-column justify-content-center align-items-center ${classes.fullscreen} ${classes.background} ${classes.mainFont}`}>
            <div className="d-flex align-items-center">
                <Logo />
                <Title />
            </div>
            <StandardButton className="m-2" text={"Log In"} onClick={() => loginWithRedirect()} />
        </div>
    )
}

export default LoginScreen