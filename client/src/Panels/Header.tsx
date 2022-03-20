import Logo from '../Components/Logo'
import Title from '../Components/Title'
import classes from './Header.module.scss'

const Header = () => {
    return (
        <div className={`d-flex align-items-center ${classes.background} ${classes.shadow}`}>
            <div className="px-3">
                <Logo />
            </div>
            <Title />
        </div>
    )
}

export default Header