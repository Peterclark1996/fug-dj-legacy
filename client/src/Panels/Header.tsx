import classes from './Header.module.scss'

const Header = () => {
    return (
        <div className={`d-flex align-items-center ${classes.background} ${classes.shadow}`}>
            <i className="fa-solid fa-cow mx-4" />
            <div>
                <h1 className="py-2 user-select-none">Fug DJ</h1>
            </div>
        </div>
    )
}

export default Header