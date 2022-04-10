import TagData from "../../Types/TagData"
import classes from "./Tag.module.scss"

type TagProps = {
    tag: TagData,
    onClick: () => void
}

const Tag = ({ tag, onClick }: TagProps) => {
    return (
        <div
            className={`d-flex align-items-center me-1 pointer ${classes.rounded} ${classes.shadow} ${classes.smallFont}`}
            role="button"
            style={{ "backgroundColor": `#${tag.colourHex}` }}
            onClick={onClick}
        >
            <span className="d-flex align-items-center ps-1 mb-1">{tag.name}</span>
            <div className={`bg-secondary px-1 mx-1 ${classes.rounded}`}>
                <i className="fa-solid fa-xmark" />
            </div>
        </div>
    )
}

export default Tag