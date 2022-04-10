import TagData from "../../Types/TagData"
import classes from "./Tag.module.scss"

type TagProps = {
    tag: TagData,
    onClick: () => void
}

const Tag = ({ tag, onClick }: TagProps) => {
    return (
        <div
            className={`px-2 me-1 pointer ${classes.rounded} ${classes.shadow} ${classes.smallFont}`}
            role="button"
            style={{ "backgroundColor": `#${tag.colourHex}` }}
            onClick={onClick}
        >
            {tag.name}
        </div>
    )
}

export default Tag