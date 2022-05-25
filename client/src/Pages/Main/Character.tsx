import ConnectedUser from "../../Types/ConnectedUser"

type Props = {
    user: ConnectedUser
}

const Character = ({ user }: Props) => {
    return (
        <div className="d-flex align-items-center justify-content-center position-relative" style={{ left: `calc(${user.x}% - 50px)`, top: `calc(${user.y}% - 50px)`, width: "100px", height: "100px" }}>
            <div className="d-flex flex-column align-items-center">
                {user.name}
                <i className="fa-solid fa-face-meh" />
            </div>
        </div>
    )
}

export default Character