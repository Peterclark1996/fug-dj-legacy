import TagData from "../../Types/TagData"
import classes from "./Tag.module.scss"

type TagProps = {
    tag: TagData,
    onClick: () => void
}

const Tag = ({ tag, onClick }: TagProps) => {
    return (
        <div
            className={`d-flex align-items-center px-1 me-1 ${classes.rounded} ${classes.shadow} ${classes.smallFont}`}
            role="button"
            style={{ "backgroundColor": `#${tag.colourHex}` }}
            onClick={onClick}
        >
            <span className={classes.tagText}>{tag.name}</span>
            <i className="ms-1 text-light-grey fa-solid fa-circle-xmark" />
        </div>
    )
}

export default Tag